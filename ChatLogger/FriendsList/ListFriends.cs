/*  
 ▄▀▀▄ ▄▀▄  ▄▀▀█▄▄▄▄  ▄▀▀▄▀▀▀▄  ▄▀▄▄▄▄   ▄▀▀▄ ▄▀▀▄  ▄▀▀▄▀▀▀▄  ▄▀▀▄ ▀▀▄ 
█  █ ▀  █ ▐  ▄▀   ▐ █   █   █ █ █    ▌ █   █    █ █   █   █ █   ▀▄ ▄▀ 
▐  █    █   █▄▄▄▄▄  ▐  █▀▀█▀  ▐ █      ▐  █    █  ▐  █▀▀█▀  ▐     █   
  █    █    █    ▌   ▄▀    █    █        █    █    ▄▀    █        █   
▄▀   ▄▀    ▄▀▄▄▄▄   █     █    ▄▀▄▄▄▄▀    ▀▄▄▄▄▀  █     █       ▄▀    
█    █     █    ▐   ▐     ▐   █     ▐             ▐     ▐       █     
▐    ▐     ▐                  ▐                                 ▐   
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MercuryBOT.FriendsList
{

    //Thanks to https://github.com/waylaidwanderer/Mist
    class ListFriends
    {

        string name = "";
        string nickname = "";
        ulong sid = 0;
        string status = "";
        static Object locker = new Object();
        static List<ListFriends> list = new List<ListFriends>();

        public ListFriends(string name, ulong sid, string nickname, string status)
        {
            this.name = name;
            this.sid = sid;
            this.nickname = nickname;
            this.status = status;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; }
        }

        public ulong SID
        {
            get { return sid; }
            set { sid = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public static void Add(string name, ulong sid, string nickname = "", string status = "Offline")
        {
            lock (locker)
            {
                ListFriends item = new ListFriends(name, sid, nickname, status);
                list.Add(item);
            }
        }

        public static void Remove(ulong sid)
        {
            lock (locker)
            {
                ListFriends item = list.Find(x => x.SID == sid);
                list.Remove(item);
            }
        }

        public static void Clear()
        {
            lock (locker)
            {
                list.Clear();
            }
        }

        public static void UpdateStatus(ulong sid, string status)
        {
            lock (locker)
            {
                ListFriends item = null;
                try
                {
                    item = list.Find(x => x.SID == sid);

                    if (item != null)
                    {
                        item.Status = status;
                    }
                }
                catch
                {
                    // Friends form hasn't been initialized yet, so let's not worry about it
                }
            }
        }

        public static void UpdateName(ulong sid, string name)
        {
            lock (locker)
            {
                ListFriends item = null;
                try
                {
                    item = list.Find(x => x.SID == sid);
                    item.Name = name;
                }
                catch
                {
                    // Friends form hasn't been initialized yet, so let's not worry about it
                }
            }
        }

        public static ulong GetSID(string name)
        {
            lock (locker)
            {
                ListFriends item = null;
                try
                {
                    item = list.Find(x => x.name == name);
                    return item.sid;
                }
                catch
                {

                }
                return 0;
            }
        }

        static internal List<ListFriends> Get(string name)
        {
            lock (locker)
            {
                name = name.ToLower();
                List<ListFriends> returnList = new List<ListFriends>();
                foreach (ListFriends item in list)
                {
                    if (item.name.ToLower().Contains(name))
                        returnList.Add(item);
                }
                return returnList;
            }
        }

        static internal ListFriends GetFriend(ulong steamId)
        {
            lock (locker)
            {
                var item = list.Find(x => x.SID == steamId);
                if (item != null)
                {
                    if (item.status == "LookingToTrade")
                    {
                        var temp = new ListFriends(item.Name, item.SID, item.Nickname, item.Status);
                        temp.status = "Looking to Trade";
                        return temp;
                    }
                    if (item.status == "LookingToPlay")
                    {
                        var temp = new ListFriends(item.Name, item.SID, item.Nickname, item.Status);
                        temp.status = "Looking to Play";
                        return temp;
                    }
                    if (item.status == "Online")
                    {
                        var temp = new ListFriends(item.Name, item.SID, item.Nickname, item.Status);
                        temp.status = "online";
                        return temp;
                    }
                    if (item.status == "Offline")
                    {
                        var temp = new ListFriends(item.Name, item.SID, item.Nickname, item.Status);
                        temp.status = "offline";
                        return temp;
                    }
                }
                return item;
            }
        }

        static internal List<ListFriends> Get(bool onlineOnly = false)
        {
            lock (locker)
            {
                List<ListFriends> returnList = new List<ListFriends>();
                foreach (ListFriends item in list)
                {
                    if (item.status == "Online")
                        returnList.Add(item);
                }
                foreach (ListFriends item in list)
                {
                    if (item.status == "LookingToTrade")
                    {
                        var temp = new ListFriends(item.Name, item.SID, item.Nickname, item.Status);
                        temp.status = "Looking to Trade";
                        returnList.Add(temp);
                    }
                }
                foreach (ListFriends item in list)
                {
                    if (item.status == "LookingToPlay")
                    {
                        var temp = new ListFriends(item.Name, item.SID, item.Nickname, item.Status);
                        temp.status = "Looking to Play";
                        returnList.Add(temp);
                    }
                }
                foreach (ListFriends item in list)
                {
                    if (item.status == "Busy")
                        returnList.Add(item);
                }
                foreach (ListFriends item in list)
                {
                    if (item.status == "Away")
                        returnList.Add(item);
                }
                foreach (ListFriends item in list)
                {
                    if (item.status == "Snooze")
                        returnList.Add(item);
                }
                if (!onlineOnly)
                {
                    foreach (ListFriends item in list)
                    {
                        if (item.status == "Offline")
                            returnList.Add(item);
                    }
                }
                return returnList;
            }
        }
    }
}