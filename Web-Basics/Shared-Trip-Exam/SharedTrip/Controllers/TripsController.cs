using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Data;
using SharedTrip.Models.Trips;
using SharedTrip.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedTrip.Data.Models;
using System.Globalization;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IValidator validator;

        public TripsController(ApplicationDbContext data, IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse All()
        {
            if (!this.User.IsAuthenticated)
            {
                return Redirect("/Users/Login");
            }

            var tripsQuery = this.data
                .Trips
                .AsQueryable();


            var trips = tripsQuery.Select(t => new TripsListingViewModel
            {
                Id = t.Id,
                StartPoint = t.StartPoint,
                DepartureTime = t.DepartureTime.ToLocalTime().ToString("F"),
                EndPoint = t.EndPoint,
                Seats = t.Seats
            })
                .ToList();

            return this.View(trips);
        }

        [Authorize]
        public HttpResponse Add()
        {
            if (!this.User.IsAuthenticated)
            {
                return Redirect("/Users/Login");
            }
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Add(AddTripsViewModel model)
        {
            var modelErrors = this.validator.ValidateTrip(model);

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var depapartureParse = DateTime.ParseExact(model.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);

            var trip = new Trip
            {
                DepartureTime = depapartureParse,
                Description = model.Description,
                EndPoint = model.EndPoint,
                StartPoint = model.StartPoint,
                ImagePath = model.ImagePath,
                Seats = model.Seats,

            };

            this.data.Trips.Add(trip);

            this.data.SaveChanges();

            return Redirect("/Trips/All");
        }

        public HttpResponse Details(string tripId)
        {
            var trip = this.data
                .Trips
                .Where(x => x.Id == tripId)
                .Select(x => new TripDetailsViewModel()
           {
               Id = x.Id,
               StartPoint = x.StartPoint,
               EndPoint = x.EndPoint,
               DepartureTime = x.DepartureTime.ToString("s"),
               Description = x.Description,
               Seats = x.Seats,
               ImagePath = x.ImagePath
           })
           .FirstOrDefault();

            return this.View(trip);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!User.IsAuthenticated)
            {
                return Redirect("/");
            }

            var user = this.User;

            var userExistInTripCheck = this.data
                .UserTrips
                .FirstOrDefault(u => u.UserId == user.Id && u.TripId == tripId);

            if (userExistInTripCheck != null)
            {
                return this.Error("This trip has already been added");
            }

            var newUserToTrip = new UserTrip { TripId = tripId, UserId = user.Id };

            var currentTrip = this.data
                .Trips
                .FirstOrDefault(t => t.Id == tripId);

            currentTrip.Seats -= 1;

            if (currentTrip.Seats != 0 || currentTrip.Seats == 0)
            {
                this.data.Trips.Update(currentTrip);
                this.data.UserTrips.Add(newUserToTrip);

                this.data.SaveChanges();
            }
            else if (currentTrip.Seats < 0)
            {
                return this.Error("Тhere are no free seats left.");
            }

            return this.Redirect("/Trips/All");
        }
    }
}
