using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yari.Audio
{

	public interface AudioData
	{

		AudioClip CreateClip();

		float GetLengthInMill();

	}

}