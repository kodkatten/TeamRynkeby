using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
	public class ContributedInventoryItemModel
	{
        public int Id { get; set; }
        public string Name { get; private set; }
        public int RequiredQuantity { get; private set; }
        public int TotalContributedQuantity { get; private set; }
        public int ContributedByUser { get; private set; }

	    public ContributedInventoryItemModel(ActivityItem activityItem, Activity activityData, User user)
	    {
	        Id = activityItem.Id;
	        Name = activityItem.Name;
	        RequiredQuantity = activityItem.Quantity;
            TotalContributedQuantity = GetTotalContributedQuantity(activityItem, activityData);
	        ContributedByUser = GetContributedByUser(activityItem, activityData, user);

	    }

	    private int GetContributedByUser(ActivityItem activityItem, Activity activityData, User user)
	    {
	        return activityData.Sessions.Sum(session => session.Volunteers.Where(v => v.Id == user.Id).Sum(volunteer => volunteer.Items.Where(i => i.Item.Id == activityItem.Id).Sum(item => item.Quantity)));
	    }

	    private int GetTotalContributedQuantity(ActivityItem activityItem, Activity activityData)
        {
            return activityData.Sessions.Sum(session => session.Volunteers.Sum(volunteer => volunteer.Items.Where(i => i.Item.Id == activityItem.Id).Sum(item => item.Quantity)));
        }
	}
}