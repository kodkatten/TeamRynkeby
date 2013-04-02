﻿using System;
using System.Collections.Generic;
using EventBooking.Data;
using EventBooking.Data.Entities;

namespace EventBooking.Services
{
	public interface ISecurityService
	{
		bool IsLoggedIn();
		User GetCurrentUser();
		IEnumerable<string> GetRolesForCurrentUser();
		void SignOff();
		bool SignIn(string username, string password);
		void CreateUserAndAccount(string email, string password, DateTime created);
	    User GetUser(string electronicMailAddress);
		bool ToogleTeamPowerUser(int userId, int teamId);
		bool IsCurrentUserAdministrator();
		bool IsCurrentUserAdministratorOrPowerUser();
		bool IsCurrentUserAdministratorOrPowerUser(int teamId);
		string GetPowerUserRoleForTeam(Team team);
		bool ToogleAdministrator(int userId);
	}
}