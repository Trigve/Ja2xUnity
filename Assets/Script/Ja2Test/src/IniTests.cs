using System.IO;
using System.Text;

using NUnit.Framework;

using Ja2;

/// <summary>
/// Various tests for <see cref="Ja2.IniFile"/>.
/// </summary>
public class IniTests
{
#region Constants
	/// <summary>
	/// Testing INI content.
	/// </summary>
	private const string TestIniData = @"[Test]
key1 = value
key2 = 1
key3 = -1
key4 = 1.2
key5 = true
key6 = false
";
#endregion

#region Methods Public
	/// <summary>
	/// Parsing test.
	/// </summary>
	[Test]
	public void IniTestParse()
	{
		using var stream_reader = new StreamReader(
			new MemoryStream(Encoding.UTF8.GetBytes(TestIniData))
		);

		var ini_file = new IniFile(stream_reader);

		// Test for valid keays
		Assert.AreEqual("value",
			ini_file.GetStringProperty("test",
				"key1"
			)
		);
		Assert.AreEqual(1,
			(int?)ini_file.GetIntProperty("test",
				"key2"
			)
		);
		Assert.AreEqual(-1,
			(int?)ini_file.GetIntProperty("test",
				"key3"
			)
		);
		Assert.AreEqual(1.2f,
			(float?)ini_file.GetFloatProperty("test",
				"key4"
			)
		);
		Assert.AreEqual(true,
			ini_file.GetBoolProperty("test",
				"key5"
			)
		);
		Assert.AreEqual(false,
			ini_file.GetBoolProperty("test",
				"key6"
			)
		);

		// Non-valid keys
		Assert.IsNull(
			ini_file.GetStringProperty("test",
				"x"
			)
		);
		Assert.AreEqual(string.Empty,
			ini_file.GetStringProperty("test",
				"x",
				string.Empty
			)
		);

		Assert.IsNull(
			ini_file.GetIntProperty("test",
				"x"
			)
		);
		Assert.AreEqual(-1,
			ini_file.GetIntProperty("test",
				"x",
				-1
			)
		);

		Assert.IsNull(
			ini_file.GetFloatProperty("test",
				"x"
			)
		);
		Assert.AreEqual(-1,
			ini_file.GetFloatProperty("test",
				"x",
				-1
			)
		);

		Assert.IsNull(
			ini_file.GetBoolProperty("test",
				"x"
			)
		);
		Assert.AreEqual(false,
			ini_file.GetBoolProperty("test",
				"x",
				false
			)
		);
	}

	/// <summary>
	/// Saving as text.
	/// </summary>
	[Test]
	public void IniTestSave()
	{
		using var stream_reader = new StreamReader(
			new MemoryStream(Encoding.UTF8.GetBytes(TestIniData))
		);

		var ini_file = new IniFile(stream_reader);

		string data_str = GenerateString(ini_file);

		Assert.AreEqual(@"[Test]
key1 = value
key2 = 1
key3 = -1
key4 = 1.2
key5 = true
key6 = false

",
			data_str
		);
	}

	/// <summary>
	/// Manual population of the items.
	/// </summary>
	[Test]
	public void IniTestManual()
	{
		var ini_file = new IniFile();

		ini_file.SetProperty("Test",
			"key1",
			"value"
		);
		ini_file.SetProperty("Test",
			"key2",
			1
		);
		ini_file.SetProperty("Test",
			"key3",
			-1
		);
		ini_file.SetProperty("Test",
			"key4",
			1.2f
		);
		ini_file.SetProperty("Test",
			"key5",
			true
		);
		ini_file.SetProperty("Test",
			"key6",
			false
		);

		string data_str = GenerateString(ini_file);

		Assert.AreEqual(@"[Test]
key1 = value
key2 = 1
key3 = -1
key4 = 1.2
key5 = true
key6 = false

",
			data_str
		);
	}
#endregion

#region Methods Private
	/// <summary>
	/// Generate the string from the INI file.
	/// </summary>
	/// <param name="IniFile">IniFile instance.</param>
	/// <returns></returns>
	private static string GenerateString(IniFile IniFile)
	{
		var mem_stream = new MemoryStream();
		using var writer = new StreamWriter(mem_stream);
		IniFile.Write(writer);
		writer.Flush();

		mem_stream.Seek(0, SeekOrigin.Begin);

		return Encoding.UTF8.GetString(
			mem_stream.ToArray()
		);
	}
#endregion
}
