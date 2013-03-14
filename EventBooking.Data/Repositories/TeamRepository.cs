using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using EventBooking.Data.Entities;

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

			var existingTeam = context.Teams.FirstOrDefault(x => x.Name == name);

			if (existingTeam != null)
			{
				throw new ArgumentException("Team with that name already exist");
			}

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

			return context.Users.Where(u => u.Team.Id == teamId);
		}
	}
}
