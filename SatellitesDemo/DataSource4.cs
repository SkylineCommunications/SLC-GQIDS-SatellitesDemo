using System;
using System.Collections.Generic;
using System.Linq;
using Satellites;
using Skyline.DataMiner.Analytics.GenericInterface;

namespace Demo
{
	/// <summary>
	/// Represents a data source.
	/// See: https://aka.dataminer.services/gqi-external-data-source for a complete example.
	/// </summary>
	[GQIMetaData(Name = "Satellites Demo v4: with input arguments")]
	public sealed class DataSource4 : IGQIDataSource, IGQIOnInit, IGQIOnDestroy, IGQIOnPrepareFetch, IGQIInputArguments
	{
		// Define a time argument
		private readonly GQIDateTimeArgument _timeArgument = new GQIDateTimeArgument("Time");

		// Define a field to store the logger
		private IGQILogger _logger;

		// Define a field to store satellites
		private IReadOnlyList<Satellite> _satellites;

		// Define a field to store the time
		private DateTime _time;

		public OnInitOutputArgs OnInit(OnInitInputArgs args)
		{
			// Store the logger
			_logger = args.Logger;
			_logger.Information("Initialized");

			return default;
		}

		public GQIArgument[] GetInputArguments()
		{
			// Give the arguments to the GQI framework
			return new GQIArgument[]
			{
				_timeArgument,
			};
		}

		public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
		{
			// Store the time argument value if specified
			if (!args.TryGetArgumentValue(_timeArgument, out _time))
			{
				// Otherwise store the current time
				_time = DateTime.UtcNow;
			}

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
			var position = satellite.GetPosition(_time);

			var cells = new[]
			{
				new GQICell { Value = satellite.Name },
				new GQICell { Value = position.Latitude },
				new GQICell { Value = position.Longitude },
			};
			return new GQIRow(cells);
		}

		public OnDestroyOutputArgs OnDestroy(OnDestroyInputArgs args)
		{
			_logger.Information("Destroyed");
			return default;
		}
	}
}
