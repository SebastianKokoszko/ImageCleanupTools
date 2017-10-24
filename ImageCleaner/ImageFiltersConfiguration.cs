using System.Configuration;

namespace ImageCleaner
{
	class ImageFiltersConfiguration : ConfigurationSection
	{
		public static ImageFiltersConfiguration GetConfig()
		{
			return (ImageFiltersConfiguration)ConfigurationManager.GetSection(nameof(ImageFiltersConfiguration)) ?? new ImageFiltersConfiguration();
		}


		[ConfigurationProperty(nameof(ImageFilters))]
		[ConfigurationCollection(typeof(ImageFilters), AddItemName = nameof(ImageFilter))]
		public ImageFilters ImageFilters
		{
			get
			{
				return this[nameof(ImageFilters)] as ImageFilters;
			}
		}
	}
}
