using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityBuilder {
    public static class Builder {
        public static void Build() {
            Console.WriteLine("Build is Called!!");
            BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayerHandler);
        }
        
        public static void BuildPlayerHandler(BuildPlayerOptions options) {
            BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayerHandler);
           // options.locationPathName = "build/Android";
            Console.WriteLine("yeah");
            BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
        }
    }
}
