public ActionResult ViewUserDetails(string id)
        {
            // Grabs the current user ID
            var userId = id;
            // Grabs all information in Db that have the same user ID as the one logging in
            var userInfo = db.Users.Where(w => w.Id == userId);
            // Creates an empty list of UserDetailsViewModel
            List<UserDetailsViewModel> userDetails = new List<UserDetailsViewModel>();

            foreach (var user in userInfo)
            {
                // for every user, only grab the parameter fields
                userDetails.Add(new UserDetailsViewModel(user.FirstName, user.LastName, user.Address, user.HireDate.ToShortDateString(), user.Department, user.Position, user.HourlyPayRate, user.Fulltime));
            }

            return PartialView(userDetails);
        }