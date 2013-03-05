using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace EventBooking.Data.Repositories
{
	public class TeamRepository : ITeamRepository
	{
		private readonly IEventBookingContext context;

		public TeamRepository(IEventBookingContext context)
		{
			this.context = context;
		}

		public IEnumerable<Team> GetTeams()
		{
			return context.Teams.Where(x => x.IsDeleted == false).ToList();
		}

		public Team Get(int teamId)
		{
			return context.Teams
						  .Where(t => t.Id == teamId)
						  .Include("Activities")
						  .Include("Activities.Coordinator")
						  .Include("Activities.Sessions")
						  .Include("Activities.Sessions.Volunteers")
						  .First();
		}
		public Team CreateTeam(string name)
		{
			var entity = new Team { Name = name };
			context.Teams.Add(entity);
			context.SaveChanges();
			return entity;
		}

		public Team TryGetTeam(int id)
		{
			return context.Teams.FirstOrDefault(x => x.Id == id);
		}

		public void DeleteTeam(int teamId)
		{
			var team = context.Teams.FirstOrDefault(x => x.Id == teamId);
			if (team != null)
			{
				foreach (var volonteer in team.Volunteers)
				{
					volonteer.Team = null;
				}
				team.IsDeleted = true;
				context.SaveChanges();
			}
		}

		public IQueryable<User> GetTeamMembers(int teamId)
		{
			if (teamId == 0)
			{
				throw new ArgumentNullException("teamId");
			}

			return this.context.Users.Where(u => u.Team.Id == teamId);
		}

		public void RemoveAsTeamAdmin(int userId, int teamId)
		{
			var team = GetTeamOrThrow(teamId);
			var user = GetUserOrThrow(userId);

			team.TeamAdmins.Remove(user);
			context.SaveChanges();
		}

		public void AddAsTeamAdmin(int userId, int teamId)
		{
			var team = GetTeamOrThrow(teamId);
			var user = GetUserOrThrow(userId);

			team.TeamAdmins.Add(user);
			user.AdminInTeams.Add(team);
			context.SaveChanges();
		}

		private User GetUserOrThrow(int userId)
		{
			var user = context.Users.FirstOrDefault(x => x.Id == userId);

			if (user == null)
			{
				throw new ArgumentException("Unknown user");
			}

			return user;
		}

		private Team GetTeamOrThrow(int teamId)
		{
			var team = context.Teams.FirstOrDefault(x => x.Id == teamId);
			if (team == null)
			{
				throw new ArgumentException("Unknown team");
			}

			return team;
		}
	}
}
