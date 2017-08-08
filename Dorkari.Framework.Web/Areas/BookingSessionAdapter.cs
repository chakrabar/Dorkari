using Dorkari.Framework.Web.Areas.SecureStatefulSPAFramework.Models;
using Dorkari.Framework.Web.Areas.SecureStatefulSPAFramework.Models.Enums;
using Dorkari.Framework.Web.Helpers;
using Dorkari.Framework.Web.Helpers.Contracts;
using System;

namespace Dorkari.Framework.Web.Areas
{
    public class BookingSessionAdapter
    {
        const string _BOOKING_SESSION_KEY = "UserBookingData";

        readonly IStateHelper _sessionHelper;

        public BookingSessionAdapter()
        {
            _sessionHelper = new WebSessionHelper();
        }

        internal Tuple<string, BookingViewModel> GetCurrentViewDetails()
        {
            var model = GetSessionData();
            return new Tuple<string, BookingViewModel>(model.NextStep.ToString(), BookingViewModel.GetViewModel(model));
        }

        internal Tuple<string, BookingViewModel> UpdateSession(string email)
        {
            var sessionModel = GetSessionData();
            if (sessionModel.NextStep == BookTicketStep.Welcome)
                sessionModel = new BookingSessionModel();
            if (!string.IsNullOrEmpty(email))
            {
                sessionModel.UserEmail = email; //TODO: write actual logic
            }
            SetSessionData(sessionModel);
            return new Tuple<string, BookingViewModel>(sessionModel.NextStep.ToString(), BookingViewModel.GetViewModel(sessionModel));
        }

        private void SetSessionData(BookingSessionModel data)
        {
            _sessionHelper.AddData(_BOOKING_SESSION_KEY, data);
        }

        private BookingSessionModel GetSessionData()
        {
            var data = _sessionHelper.GetData<BookingSessionModel>(_BOOKING_SESSION_KEY);
            return data ?? new BookingSessionModel();
        }
    }
}