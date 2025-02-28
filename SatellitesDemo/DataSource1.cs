using System.Linq;
using Satellites;
using Skyline.DataMiner.Analytics.GenericInterface;

namespace Demo
{
	[GQIMetaData(Name = "Satellites Demo v1: static data")]
	public sealed class DataSource1 : IGQIDataSource
	{
		public GQIColumn[] GetColumns()
		{
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
	}
}
