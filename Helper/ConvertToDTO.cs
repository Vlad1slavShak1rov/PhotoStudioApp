using PhotoStudioApp.Model;
using PhotoStudioApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhotoStudioApp.Helper
{
    public static class ConvertToDTO
    {
        public static CustomerDTO ToCustomerDTO(Customer customer)
        {
            return new CustomerDTO()
            {
                ID = customer.ID,
                Name = customer.Name,
                Balance = customer.Balance,
                SecondName = customer.SecondName,
                LastName = customer.LastName,
                Contact = customer.Contact,
                UserID = customer.UserID,
            };
        }
        public static BookingServiceDTO ToBookingDTO(Booking booking)
        {
            return new BookingServiceDTO()
            {
                ID = booking.ID,
                CustomerID = booking.ID,
                PhotographID = booking.ID,
                VisagisteID = booking.ID,
                HallID = booking.ID,
                ServiceID = booking.ID,
                AdditionalServicesID = booking.AdditionalServicesID,
                CostServices = booking.CostServices,
                DateBooking = booking.DateBooking
            };
        }

        public static PaymentDTO ToPaymentsDTO(Payment payment)
        {
            return new PaymentDTO()
            {
                ID = payment.ID,
                BookingID = payment.BookingID,
                Amount = (decimal)payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                PaymentDate = DateTime.Now,
            };
        }

        public static AdditionalServiceDTO ToAdditionalServiceDTO(AdditionalService additionalService)
        {
            return new AdditionalServiceDTO()
            {
                ID = additionalService.ID,
                ServiceName = additionalService.ServiceName,
                Description = additionalService.Description,
                Cost = additionalService.Cost,
            };
        }

        public static ServiceDTO ToServiceDTO(Services service)
        {
            return new ServiceDTO()
            {
                ID = service.ID,
                ServiceName = service.ServiceName,
                Description = service.Description,
                CostService = service.CostService,
            };
        }

        public static WorkerDTO ToWorkerDTO(Worker worker)
        {
            return new WorkerDTO()
            {
                ID = worker.ID,
                UserID = worker.UserID,
                Name = worker.Name,
                SecondName = worker.SecondName,
                LastName = worker.LastName,
            };
        }

        public static ReviewDTO ToReviewDTO(Review review)
        {
            return new ReviewDTO()
            {
                ID = review.ID,
                CustomerID = review.CustomerID,
                BookingID = review.BookingID,
                Rating = review.Rating,
                ReviewDate = review.ReviewDate,
                ReviewText = review.ReviewText,
            };
        }
    }
}
