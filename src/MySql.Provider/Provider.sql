USE OpenCommunication
GO
INSERT [dbo].[Provider] ([ID], [Name], [Active],[Plan], [Details], [Type], [Logo], [PullLink], [Category], [Configuration])
VALUES ('e905382e-aa30-45fa-b434-3c9fb60a358f', N'MySql', 1, 1, N'Our MySql provider will allow you to search across all your MySql accounts.', N'cloud', N'http://immense-refuge-3500.herokuapp.com/img/providers/salesforce.png', N'http://proget.cerebro.technology/salesforce', 'Files', '{ "actions": [ { "name" : "start", "action": "javascript function"}, { "name" : "share", "action": "javascript function for share"} ] }')
GO
