using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        private readonly IImageService imageService;
        private readonly FaceServiceClient faceClient;
        private readonly ConcurrentDictionary<Guid, Person> personsCache = new ConcurrentDictionary<Guid, Person>();
        private readonly IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<HomeHub>();

        private static readonly List<string> FaceEventsSubscribers = new List<string> { GroupNames.Home, GroupNames.Hue };

        public FaceService(IImageService imageService)
        {
            this.imageService = imageService;
            faceClient = new FaceServiceClient(SubscriptionKey, HttpsWestusApiCognitiveMicrosoftComFaceV1);
        }

        private const string SubscriptionKey = "8e35e86b64fe441ab9eb63070faba908";
        private const string FriendsGroupId = "friends";
        private const float ConfidenceThreshold = 0.7f;
        private const string FacesUnknownMessage = "Faces Unknown";
        private const string HttpsWestusApiCognitiveMicrosoftComFaceV1 = "https://westus.api.cognitive.microsoft.com/face/v1.0";

        public async Task Identify(string imageId, Uri imageLocation)
        {
            var detectedFaces = await faceClient.DetectAsync(imageLocation.ToString());

            if (!detectedFaces.Any())
            {
                RaiseUnknownFaces();
                return;
            }

            var personFaces = detectedFaces.ToDictionary(f => f.FaceId, f => new PersonFace { Face = f });

            var faceIds = detectedFaces.Select(f => f.FaceId).ToArray();
            var identifyResults = await faceClient.IdentifyAsync(FriendsGroupId, faceIds, ConfidenceThreshold);

            foreach (var identifyResult in identifyResults)
            {
                var candidate = identifyResult.Candidates.FirstOrDefault(); // it's never more than one
                Person person;
                if (candidate != null && personsCache.TryGetValue(candidate.PersonId, out person))
                {
                    personFaces[identifyResult.FaceId].Person = person;
                }
            }

            // await SetImageWithFaces(imageId, personFaces.Values.Where(p => p.Person != null).ToArray());

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

        private async Task SetImageWithFaces(string imageId, PersonFace[] personFaces)
        {
            var imageBytes = await imageService.GetImage(imageId);

            using (var imageStream = new MemoryStream(imageBytes))
            using (var image = Image.FromStream(imageStream))
            {
                using (var g = Graphics.FromImage(image))
                {
                    DrawFaces(g, personFaces);
                    var newImageBytes = GetImageBytes(image);
                    imageService.SaveImageWithId($"{imageId}-faces", newImageBytes);
                }
            }
        }

        private byte[] GetImageBytes(Image image)
        {
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);

            return stream.ToArray();
        }

        private void DrawFaces(Graphics graphics, PersonFace[] personFaces)
        {
            var pen = new Pen(Color.DeepSkyBlue, 8);

            var font = new Font(FontFamily.GenericSansSerif, 60);

            foreach (var personFace in personFaces)
            {
                var faceRectangle = CreateRectangle(personFace.Face.FaceRectangle);
                var nameRectangle = new Rectangle(faceRectangle.Left - 4, faceRectangle.Top + faceRectangle.Height + 8, faceRectangle.Width + 6, 55);

                graphics.DrawRectangle(pen, faceRectangle);
                graphics.FillRectangle(Brushes.DeepSkyBlue, nameRectangle);
                graphics.DrawString(personFace.Person.Name, font, Brushes.Black, faceRectangle.Left + faceRectangle.Width / 5, faceRectangle.Top + faceRectangle.Height);
            }
        }

        private Rectangle CreateRectangle(FaceRectangle faceRectangle)
        {
            return new Rectangle(faceRectangle.Left, faceRectangle.Top, faceRectangle.Width, faceRectangle.Height);
        }


        private void RaiseUnknownFaces()
        {
            hubContext.Clients.Groups(FaceEventsSubscribers).FacesUnknown(FacesUnknownMessage);
        }

        private void RaiseFaceDetected(List<Candidate> candidates)
        {
            hubContext.Clients.Groups(FaceEventsSubscribers).FacesIdentified(CreateMessageFor(candidates));
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

        class PersonFace
        {
            public Face Face { get; set; }
            public Person Person { get; set; }
        }
    }
}
