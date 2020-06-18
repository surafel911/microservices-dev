using System;

using Xunit;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using PatientService.Models;
using PatientService.Services;
using PatientService.Controllers;

using PatientServiceTest.Models;

namespace PatientServiceTest
{
	public class PatientServiceIntegrationTest
		: IClassFixture<WebApplicationFactory<RazorPagesProject.Startup>>
	{
		private readonly WebApplicationFactory<PatientService.Startup> _factory;

		public PatientServiceIntegrationTest(WebApplicationFactory<RazorPagesProject.Startup> factory)
		{
			_factory = factory;
		}

		// TODO: Start writing integration testing.
	}
}
