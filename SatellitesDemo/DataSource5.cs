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
	[GQIMetaData(Name = "Satellites Demo v5: with real-time updates")]
	public sealed class DataSource5 : IGQIDataSource, IGQIOnInit, IGQIOnDestroy, IGQIOnPrepareFetch, IGQIUpdateable
	{
		// Define a field to store the logger
		private IGQILogger _logger;

		// Define a field to store satellites
		private IReadOnlyList<Satellite> _satellites;

		// Define a field to store the time
		private DateTime _time;

		// Define a field to store the updater
		private IGQIUpdater _updater;

		// Define a field to store the trigger
		private Trigger _trigger;

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
			// Use the satellite name as row key
			var rowKey = satellite.Name;
			var position = satellite.GetPosition(_time);

			return new GQIRow(rowKey, new[]
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

		public void OnStartUpdates(IGQIUpdater updater)
		{
			// Store the updater
			_updater = updater;

			// Create a trigger
			_trigger = new Trigger(Update);

			_logger.Information("Updates started");
		}

		private void Update()
		{
			try
			{
				// Update the time
				_time = DateTime.UtcNow;

				// For each satellite
				foreach (var satellite in _satellites)
				{
					// Create an updated row
					var updatedRow = CreateRow(satellite);

					// Send to updated row to the updater
					_updater.UpdateRow(updatedRow);
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Failed to update satellites.");
			}
		}

		public void OnStopUpdates()
		{
			// Clean up the timer
			_trigger.Dispose();

			_logger.Information("Updates stopped");
		}
	}
}
