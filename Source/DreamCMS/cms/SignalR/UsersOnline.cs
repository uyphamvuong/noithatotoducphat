using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using DreamCMS.Models;

namespace DreamCMS
{
    [HubName("userActivityHub")]
    public class UserActivityHub : Hub
    {
        /// <summary>
        /// The count of users connected.
        /// </summary>
        public static List<string> Users = new List<string>();

        /// <summary>
        /// Sends the update user count to the listening view.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        public void Send(int count, bool refreshData = false)
        {
            // Call the addNewMessageToPage method to update clients.
            var context = GlobalHost.ConnectionManager.GetHubContext<UserActivityHub>();

            //DataSent
            DataSent ds = new DataSent();
            ds.CurrentOnline = count;
            if (refreshData)
            {
                try
                {
                    ds.Count_DayView = Setting.Get("Count_DayView");
                    ds.Count_MonthView = Setting.Get("Count_MonthView");
                    ds.Count_TotalView = Setting.Get("Count_TotalView");
                    ds.RefreshData = refreshData;
                }
                catch
                {

                }
            }            
            context.Clients.All.updateUsersOnlineCount(ds);
        }

        /// <summary>
        /// The OnConnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override System.Threading.Tasks.Task OnConnected()
        {
            string clientId = GetClientId();

            if (Users.IndexOf(clientId) == -1)
            {
                Users.Add(clientId);
            }
            
            // Send the current count of users
            UpdateCountView();
            Send(Users.Count,true);            

            return base.OnConnected();
        }

        /// <summary>
        /// The OnReconnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override System.Threading.Tasks.Task OnReconnected()
        {
            string clientId = GetClientId();
            if (Users.IndexOf(clientId) == -1)
            {
                Users.Add(clientId);
            }

            // Send the current count of users
            UpdateCountView();
            Send(Users.Count);            

            return base.OnReconnected();
        }

        /// <summary>
        /// The OnDisconnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            string clientId = GetClientId();

            if (Users.IndexOf(clientId) > -1)
            {
                Users.Remove(clientId);
            }

            // Send the current count of users
            Send(Users.Count);

            return base.OnDisconnected(true);
        }

        /// <summary>
        /// Get's the currently connected Id of the client.
        /// This is unique for each client and is used to identify
        /// a connection.
        /// </summary>
        /// <returns>The client Id.</returns>
        private string GetClientId()
        {
            string clientId = "";
            if (Context.QueryString["clientId"] != null)
            {
                // clientId passed from application 
                clientId = this.Context.QueryString["clientId"];
            }

            if (string.IsNullOrEmpty(clientId.Trim()))
            {
                clientId = Context.ConnectionId;
            }

            return clientId;
        }

        public void UpdateCountView()
        {
            try
            {
                long Count_TotalView = Setting.Get("Count_TotalView") == "" ? 0 : int.Parse(Setting.Get("Count_TotalView"));
                Setting.Set("Count_TotalView", (Count_TotalView+1).ToString());

                //MonthView
                long Count_MonthView = Setting.Get("Count_MonthView") == "" ? 0 : int.Parse(Setting.Get("Count_MonthView"));
                int Current_MonthView = Setting.Get("Current_MonthView") == "" ? 0 : int.Parse(Setting.Get("Current_MonthView"));
                if (Current_MonthView != DateTime.Now.Month)
                {
                    Current_MonthView = DateTime.Now.Month; 
                    Setting.Set("Current_MonthView", DateTime.Now.Month.ToString());
                    Setting.Set("Count_MonthView", "1");
                }
                else { Setting.Set("Count_MonthView", (Count_MonthView+1).ToString()); }

                //DayView
                long Count_DayView = Setting.Get("Count_DayView") == "" ? 0 : int.Parse(Setting.Get("Count_DayView"));
                int Current_DayView = Setting.Get("Current_DayView") == "" ? 0 : int.Parse(Setting.Get("Current_DayView"));
                if (Current_DayView != DateTime.Now.Day)
                {
                    Current_MonthView = DateTime.Now.Day;
                    Setting.Set("Current_DayView", DateTime.Now.Day.ToString());
                    Setting.Set("Count_DayView", "1");
                }
                else { Setting.Set("Count_DayView", (Count_DayView + 1).ToString()); }

            }
            catch
            {

            }
        }

        struct DataSent{
            public int CurrentOnline;
            public string Count_DayView;
            public string Count_MonthView;
            public string Count_TotalView;
            public bool RefreshData;
        }

    }
}