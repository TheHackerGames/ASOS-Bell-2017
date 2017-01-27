using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackerGamesHub.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace HackerGamesHub.Services
{
    public class FaceService : IFaceService
    {
        private readonly FaceServiceClient faceClient = new FaceServiceClient(SubscriptionKey);
        private readonly ConcurrentDictionary<Guid, Person> personsCache = new ConcurrentDictionary<Guid, Person>();
        private readonly IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<HomeHub>();

        private const string SubscriptionKey = "8e35e86b64fe441ab9eb63070faba908";
        private const string FriendsGroupId = "friends";
        private const float ConfidenceThreshold = 0.7f;
        private const string FacesUnknownMessage = "Faces Unknown";

        public async Task Identify(Uri imageLocation)
        {
            var detectedFaces = await faceClient.DetectAsync(imageLocation.ToString());
            var faceIds = detectedFaces.Select(f => f.FaceId).ToArray();
            var identifyResults = await faceClient.IdentifyAsync(FriendsGroupId, faceIds, ConfidenceThreshold);

            var candidates = identifyResults.SelectMany(r => r.Candidates).ToList();

            if (!candidates.Any())
            {
                RaiseUnknownFaces();
            }
            else
            {
                RaiseFaceDetected(candidates);
            }
        }

        private void RaiseUnknownFaces()
        {
            hubContext.Clients.Group(GroupNames.Home).FacesUnknown(FacesUnknownMessage);
        }

        private void RaiseFaceDetected(List<Candidate> candidates)
        {
            hubContext.Clients.Group(GroupNames.Home).FacesIdentified(CreateMessageFor(candidates));
        }

        private string CreateMessageFor(IReadOnlyCollection<Candidate> candidates)
        {
            var builder = new StringBuilder();

            var names = new List<string>();
            foreach (var candidate in candidates)
            {
                Person person;
                if (personsCache.TryGetValue(candidate.PersonId, out person))
                {
                    names.Add(person.Name);
                }
            }

            var namesJoined = string.Join(", ", names);
            builder.Append(namesJoined);
            
            builder.Append(" appear");

            if (candidates.Count == 1)
            {
                builder.Append("s");
            }
            builder.Append(" to be at your door.");

            return builder.ToString();
        }

        public async void LoadPersons()
        {
            var persons = await faceClient.GetPersonsAsync(FriendsGroupId);

            foreach (var person in persons)
            {
                personsCache.TryAdd(person.PersonId, person);
            }
        }
    }
}
