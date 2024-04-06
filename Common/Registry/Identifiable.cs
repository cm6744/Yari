namespace Yari.Common.Registry
{

	public interface Identifiable
	{

		public Identity Registry { get; set; }

		public void OnRegistry();

	}

}