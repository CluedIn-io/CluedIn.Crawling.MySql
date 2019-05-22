USE OpenCommunication
GO
INSERT [dbo].[Provider] ([ID], [Name], [Active],[Plan], [Details], [Type], [Logo], [PullLink], [Category], [Configuration], [RawGuide], [RawGuide])
VALUES ('620bbc66-0545-4490-a74d-bf2a2f4299be', N'MySql', 1, 1, N'Our MySql provider will allow you to search across all your MySql databases.', N'cloud', N'http://immense-refuge-3500.herokuapp.com/img/providers/salesforce.png', N'http://proget.cerebro.technology/salesforce', 'Files', '{ "actions": [ { "name" : "start", "action": "javascript function"}, { "name" : "share", "action": "javascript function for share"} ] }', '{ "Instructions": "You will need to paste in your database connection string to connect to your database.",  "Value": ["", ""] }', '{ "Instructions": "You will need to paste your connection string here to authenticate to your MySql database.",  "Value": ["", ""], "Details": "Our MySql provider will allow you to search across all your MySql databases." }')
GO
