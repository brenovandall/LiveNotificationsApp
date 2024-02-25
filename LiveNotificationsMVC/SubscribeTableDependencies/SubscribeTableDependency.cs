using LiveNotificationsMVC.Hubs;
using LiveNotificationsMVC.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlConnector;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Messages;

namespace LiveNotificationsMVC.SubscribeTableDependencies;
// para todas as ações, utilizei comandos de inserção no banco de dados para as notificações serem exibidas
// abaixo, estão os dois comandos, para um usuário específico, ou para todos
//INSERT INTO Notifications(username, message, messageType, notificationDateTime)
//VALUES('Breno', 'Teste', 'Personal', GETDATE());

//INSERT INTO Notifications(username, message, messageType, notificationDateTime)
//VALUES('Breno', 'Teste', 'All', GETDATE());

public class SubscribeTableDependency : ISubscribeTableDependency
{
    private readonly NotificationHub _hub;
    SqlTableDependency<Notifications> _tableDependency;
    public SubscribeTableDependency(NotificationHub hub)
    {
        _hub = hub;
    }

    public void CheckForDependencies(string connectionString)
    {
        _tableDependency = new SqlTableDependency<Notifications>(connectionString);
        _tableDependency.OnError += _tableDependency_OnError;
        _tableDependency.OnChanged += _tableDependency_OnChanged;
        _tableDependency.Start();
    }

    private async void _tableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Notifications> e)
    {
        if(e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
        {
            var notification = new Notifications();
            notification = e.Entity;
            await HandleNotificationAsync(notification);
        }
    }

    private async Task HandleNotificationAsync(Notifications notification)
    {
        string mes = notification.Message;
        string user = notification.Username;
        if (notification.MessageType == "All")
        {
            await _hub.SendNotificationToAll(mes);
        }
        else if (notification.MessageType == "Personal")
        {
            await _hub.SendNotificationToClient(user, mes);
        }
    }

    private void _tableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
    {
        Console.WriteLine($"{nameof(Notifications)} SqlTableDependency error: {e.Message}");
    }
}
