using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserManager
{
    public static int userID;
    public static string username;
    
    public static bool LoggedIn => username != null;

    public static void LogOut()
    {
        username = null;
    }
}
