public ActionResult UserViewEvents()    // Gets called on AccountController/LoginRoute
        {
            // Grabs the current user ID
            var userId = User.Identity.GetUserId();
            // Grabs all events in Db that have the same user ID as the one logging in
            var workTimeEvents = db.WorkTimeEvents.Where(w => w.User.Id == userId);
            // Creates an empty list of WorkTimeEventViewModel
            List<WorkTimeEventViewModel> UserEventList = new List<WorkTimeEventViewModel>();
            
            foreach (var item in workTimeEvents)
            {
                // for every event in workTimeEvents, grab only the Start, End, & Note
                UserEventList.Add(new WorkTimeEventViewModel(item.Start, item.End, item.Note));
            }
            return View("ViewUserEvents", UserEventList);
        }