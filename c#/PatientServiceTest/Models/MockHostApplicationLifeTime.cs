using System.Threading;

using Microsoft.Extensions.Hosting;

namespace PatientServiceTest.Models
{
	public class MockHostApplicationLifetime : IHostApplicationLifetime
	{
		public CancellationToken ApplicationStarted { get; }

		public CancellationToken ApplicationStopped { get; }
		
		public CancellationToken ApplicationStopping { get; }

		public void StopApplication()
		{
		}
	}
}
