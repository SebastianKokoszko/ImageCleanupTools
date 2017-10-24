namespace TextRecognitionWrapper
{
	using System;
	using System.Drawing;
	using System.Text;
	using Tesseract;

	/// <summary>
	/// The <see cref="TextIsolator"/> class.
	/// </summary>
	public sealed class TextIsolator
	{
		/// <summary>
		/// Synchronization object.
		/// </summary>
		private static readonly object SyncRoot = new object();

		/// <summary>
		/// The single instance.
		/// </summary>
		private static volatile TextIsolator instance;

		/// <summary>
		/// Prevents a default instance of the <see cref="TextIsolator"/> class from being created.
		/// </summary>
		private TextIsolator()
		{
		}

		/// <summary>
		/// Gets the single instance.
		/// </summary>
		public static TextIsolator Instance
		{
			get
			{
				if (instance == null)
				{
					lock (SyncRoot)
					{
						if (instance == null)
						{
							instance = new TextIsolator();
						}
					}
				}

				return instance;
			}
		}

		public String IsolateText(Bitmap bitmap)
		{			
			using (TesseractEngine engine = new TesseractEngine("./dataset", "eng", EngineMode.TesseractAndCube))
			{
				using (Page page = engine.Process(bitmap))
				{
					return page.GetText();
				}				
			}
		}
	}
}