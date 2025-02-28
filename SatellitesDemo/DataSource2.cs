using System.Linq;
using Satellites;
using Skyline.DataMiner.Analytics.GenericInterface;

namespace Demo
{
	[GQIMetaData(Name = "Satellites Demo v2: with logging")]
	public sealed class DataSource2 : IGQIDataSource, IGQIOnInit, IGQIOnDestroy
	{
		// Define a field to store the logger
		private IGQILogger _logger;

		public OnInitOutputArgs OnInit(OnInitInputArgs args)
		{
			// Store the logger
			_logger = args.Logger;
			_logger.Information("Initialized");

			return default;
		}

		public GQIColumn[] GetColumns()
		{
			_logger.Information("Defining columns...");

			// Define the columns of the data source
			var nameColumn = new GQIStringColumn("Name");
			var latitudeColumn = new GQIDoubleColumn("Latitude");
			var longitudeColumn = new GQIDoubleColumn("Longitude");

			// Give the columns to the GQI framework
			return new GQIColumn[]
			{
				nameColumn,
				latitudeColumn,
				longitudeColumn,
			};
		}

		public GQIPage GetNextPage(GetNextPageInputArgs args)
		{
			// Retrieve satellites from the API
			var satellites = SatelliteAPI.GetSatellites();

			// Create a row for each satellite
			var rows = satellites
				.Select(CreateRow)
				.ToArray();

			// Give the rows as a single page to the GQI framework
			return new GQIPage(rows);
		}

		private GQIRow CreateRow(Satellite satellite)
		{
			var position = satellite.GetPosition();

			return new GQIRow(new[]
			{
				new GQICell { Value = satellite.Name },
				new GQICell { Value = position.Latitude },
				new GQICell { Value = position.Longitude },
			});
		}

		public OnDestroyOutputArgs OnDestroy(OnDestroyInputArgs args)
		{
			_logger.Information("Destroyed");
			return default;
		}
	}
}
