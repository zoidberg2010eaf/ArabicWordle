using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NotificationsManager : Singleton<NotificationsManager>
{
    public List<Notification> notifications = new();
    
    public Tween SpawnNotification(Notification notification)
    {
        return notification.Spawn();
    }
    
    public Tween SpawnNotification(int index)
    {
        return notifications[index].Spawn();
    }
}
