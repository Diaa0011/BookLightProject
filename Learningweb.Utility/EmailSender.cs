﻿using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnweb.Utility
{
	public class EmailSender : IEmailSender
	{
		Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
		{
			//Logic for sending emails
			return Task.CompletedTask;
		}
	}
}
