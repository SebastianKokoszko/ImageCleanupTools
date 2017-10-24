using System.Configuration;

namespace ImageCleaner
{
	public class ImageFilter : ConfigurationElement
	{
		[ConfigurationProperty(nameof(Key), IsRequired = true)]
		public string Key
		{
			get
			{
				return this[nameof(Key)] as string;
			}
		}

		[ConfigurationProperty(nameof(SubdirectoryName), IsRequired = true)]
		public string SubdirectoryName
		{
			get
			{
				return this[nameof(SubdirectoryName)] as string;
			}
		}
	}
}
