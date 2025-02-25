using System.Collections.Generic;
using System.Linq;
using Satellites;
using Skyline.DataMiner.Analytics.GenericInterface;

namespace Demo
{
	[GQIMetaData(Name = "Satellites Demo v3: fetch satellites from API")]
	public sealed class DataSource3 : IGQIDataSource, IGQIOnInit, IGQIOnDestroy, IGQIOnPrepareFetch
	{
		// Define a field to store the logger
		private IGQILogger _logger;

		// Define a field to store satellites
		private IReadOnlyList<Satellite> _satellites;

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

		public OnPrepareFetchOutputArgs OnPrepareFetch(OnPrepareFetchInputArgs args)
		{
			_logger.Information("Fetching satellites...");

			// Retrieve and store satellites from the API
			_satellites = SatelliteAPI.GetSatellites();

			return default;
		}

		public GQIPage GetNextPage(GetNextPageInputArgs args)
		{
			_logger.Information("Getting the next page...");

			// Create a row for each satellite
			var rows = _satellites
				.Select(CreateRow)
				.ToArray();

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
