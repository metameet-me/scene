using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class MetameetSceneLoader : BaseSceneManager
{
	public async override Task<bool> Initialize()
	{
		base.Initialize();

		// Spawn
		await Spawn();

		return true;
	}
}