using System.Configuration;

namespace ImageCleaner
{
	public class ImageFilters : ConfigurationElementCollection
	{
		public ImageFilter this[int index]
		{
			get
			{
				return base.BaseGet(index) as ImageFilter;
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}

				this.BaseAdd(index, value);
			}
		}

		public new ImageFilter this[string responseString]
		{
			get { return (ImageFilter)BaseGet(responseString); }
			set
			{
				if (BaseGet(responseString) != null)
				{
					BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
				}

				BaseAdd(value);
			}
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new ImageFilter();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ImageFilter)element).Key;
		}
	}
}
