/* *****************************************************************************
         *                             CREATE A WORKTIMEEVENT       
         * *************************************************************************** */
        // POST: WorkTimeEvent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LoginViewModel workTimeEvent)    // workTimeEvent grabs the login email, password, and remember me
        {
            PasswordHasher ph = new PasswordHasher();
            var dt = DateTime.Now;

            // Checks Db users for email that matches the email user typed in
            ApplicationUser dbUser = db.Users.FirstOrDefault(x => x.Email == workTimeEvent.Email);

            // If email is not in Db
            if (dbUser == null)
            {
                ModelState.AddModelError("", "There was a problem with the password or username, please try again or contact you system administrator if the problem continues.");
                return View("~/Views/Account/Login.cshtml");
            }

            // Grabs user hashed PW from Db and PW user typed in and check is they match
            var result = ph.VerifyHashedPassword(dbUser.PasswordHash, workTimeEvent.Password);
            // If PW doesn't match
            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "There was a problem with the password or username, please try again or contact you system administrator if the problem continues.");
                return View("~/Views/Account/Login.cshtml");
            }

            // Grabs Db event that doesn't have an end value and matches user ID
            var notFinishedEvent = db.WorkTimeEvents.FirstOrDefault(x => x.Id == dbUser.Id && !x.End.HasValue);
            
            // If event has already been created that doesn't have an end value, update end value
            if (notFinishedEvent != null)
            {
                notFinishedEvent.End = dt;
                db.SaveChanges();
                ModelState.Clear();
                ModelState.AddModelError("", "Clock out successful at " + dt.ToShortTimeString());
                return View("~/Views/Account/Login.cshtml");
            }
            else
            {
                WorkTimeEvent clockIn = new WorkTimeEvent()
                {
                    // Uses Db to return list of emails, then grabs the first (and only) user with that email.
                    User = db.Users.Where(e => e.Email == workTimeEvent.Email).First(), 
                    Start = DateTime.Now,
                    EventID = Guid.NewGuid()
                };

                db.WorkTimeEvents.Add(clockIn);
                db.SaveChanges();
                ModelState.Clear();
                ModelState.AddModelError("", "Clock in successful at " + dt.ToShortTimeString());
                return View("~/Views/Account/Login.cshtml");
            }
        }

        /* *****************************************************************************
         *                               CHECK USER DATA
         * *************************************************************************** */
        public JsonResult CheckForUserData(string userdata) // userdata comes from function UserCheck() on Login.cshtml
        {
            // Checks Db users where the UserName == the username that was inputted by the user and stores EVERYTHING about that user
            var searchData = db.Users.Where(x => x.UserName == userdata).SingleOrDefault();

            if (searchData != null)
            {
                // Look at the Db events, grab only events that matched the userID && had a start day with todays date
                var todaysWorkTimeEvents = db.WorkTimeEvents.Where(u => u.Id == searchData.Id).Where(e => e.Start.Day == DateTime.Today.Day).ToList();

                // Grabs Db event that doesn't have an end value and matches user ID
                var notFinishedEvent = db.WorkTimeEvents.FirstOrDefault(x => x.Id == searchData.Id && !x.End.HasValue);
                
                if (notFinishedEvent != null)
                { 
                    // UserVerification (2) means there's a start time, but no end time
                    return Json(new WorkTimeEventCreateViewModel(2, todaysWorkTimeEvents), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // UserVerification (1) means there's no start or end time
                    return Json(new WorkTimeEventCreateViewModel(1, todaysWorkTimeEvents), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                // UserVerification (0) means the current user is not in our Db
                return Json(new WorkTimeEventCreateViewModel(0), JsonRequestBehavior.AllowGet);
            }
        }