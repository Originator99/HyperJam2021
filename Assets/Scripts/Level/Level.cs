using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour {
    public List<Brick> levelBricks;
    public List<Brick> safePath;


    public void ShuffleLevel() {
        if(levelBricks != null) { 
            //making a copy to shuffle items
            List<Brick> levelCopy = new List<Brick>(levelBricks);
            List<Brick> shuffle = new List<Brick>();
            while(levelCopy.Count != 0) {
                int index = Random.Range(0, levelCopy.Count);
                shuffle.Add(levelCopy[index]);
                levelCopy.RemoveAt(index);
            } //shuffling the bricks

            //reassigning the shuffled bricks to thier positions
            int count = 0;
            for(int i = 0; i < levelBricks.Count; i++) {
                Brick brick = levelBricks[i];
                if(shuffle.Count > count) {
                    brick.ChangePosition(shuffle[count].data.worldPosition);
                    count++;
                } else {
                    Debug.LogWarning("Could not shuffle completely. Items in after shuffling were less than original");
                }
            }
        }
    }
}
