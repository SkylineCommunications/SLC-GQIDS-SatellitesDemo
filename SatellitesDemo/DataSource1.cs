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
			// Define some static data
			// Each row represents a satellite
			var row1 = new GQIRow(new[]
			{
				new GQICell { Value = "Galaxy 17" },
				new GQICell { Value = 0.0 },
				new GQICell { Value = -91.0 },
			});
			var row2 = new GQIRow(new[]
			{
				new GQICell { Value = "Thor 7" },
				new GQICell { Value = 0.0 },
				new GQICell { Value = 0.8 },
			});
			var row3 = new GQIRow(new[]
			{
				new GQICell { Value = "Yamal 201" },
				new GQICell { Value = 0.0 },
				new GQICell { Value = 90.0 },
			});

			var rows = new[]
			{
				row1,
				row2,
				row3,
			};

			// Give the rows as a single page to the GQI framework
			return new GQIPage(rows);
		}
	}
}
