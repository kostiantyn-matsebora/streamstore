﻿using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StreamStore.Serialization.Tests.NewtonsoftJson
{
    public partial class NewtonsoftTestSuite
    {
        public override byte[] SerializedEvent => new byte[] { 123, 34, 84, 121, 112, 101, 34, 58, 34, 83, 116, 114, 101, 97, 109, 83, 116, 111, 114, 101, 46, 84, 101, 115, 116, 105, 110, 103, 46, 82, 111, 111, 116, 69, 118, 101, 110, 116, 44, 32, 83, 116, 114, 101, 97, 109, 83, 116, 111, 114, 101, 46, 84, 101, 115, 116, 105, 110, 103, 34, 44, 34, 68, 97, 116, 97, 34, 58, 34, 101, 121, 74, 67, 99, 109, 70, 117, 89, 50, 104, 108, 99, 121, 73, 54, 87, 51, 115, 105, 83, 87, 81, 105, 79, 106, 69, 52, 79, 67, 119, 105, 84, 109, 70, 116, 90, 83, 73, 54, 73, 107, 53, 104, 98, 87, 86, 105, 90, 68, 78, 109, 90, 87, 85, 50, 78, 83, 48, 49, 79, 84, 66, 104, 76, 84, 82, 106, 90, 84, 73, 116, 79, 68, 78, 107, 78, 67, 48, 120, 89, 122, 81, 122, 78, 122, 89, 120, 77, 122, 82, 104, 78, 122, 73, 105, 76, 67, 74, 77, 90, 87, 70, 50, 90, 88, 77, 105, 79, 108, 116, 55, 73, 107, 53, 104, 98, 87, 85, 105, 79, 105, 74, 79, 89, 87, 49, 108, 89, 84, 89, 121, 77, 106, 85, 122, 90, 87, 77, 116, 78, 50, 81, 122, 77, 67, 48, 48, 78, 84, 70, 104, 76, 84, 107, 120, 90, 71, 69, 116, 78, 68, 89, 49, 79, 84, 99, 121, 90, 109, 82, 107, 90, 87, 86, 105, 73, 105, 119, 105, 86, 109, 70, 115, 100, 87, 85, 105, 79, 106, 73, 121, 77, 83, 119, 105, 84, 110, 86, 115, 98, 70, 90, 104, 98, 72, 86, 108, 73, 106, 111, 105, 84, 110, 86, 115, 98, 70, 90, 104, 98, 72, 86, 108, 77, 106, 99, 119, 77, 50, 70, 106, 77, 106, 85, 116, 89, 109, 74, 107, 90, 67, 48, 48, 78, 68, 69, 52, 76, 87, 73, 122, 90, 84, 81, 116, 89, 84, 107, 53, 78, 122, 89, 52, 79, 68, 103, 50, 89, 122, 86, 107, 73, 110, 48, 115, 101, 121, 74, 79, 89, 87, 49, 108, 73, 106, 111, 105, 84, 109, 70, 116, 90, 84, 90, 106, 89, 109, 89, 121, 79, 84, 100, 109, 76, 84, 108, 107, 89, 84, 89, 116, 78, 71, 86, 104, 89, 121, 49, 104, 77, 68, 74, 105, 76, 84, 66, 106, 78, 106, 99, 49, 90, 106, 81, 121, 90, 109, 89, 122, 77, 121, 73, 115, 73, 108, 90, 104, 98, 72, 86, 108, 73, 106, 111, 120, 77, 122, 65, 115, 73, 107, 53, 49, 98, 71, 120, 87, 89, 87, 120, 49, 90, 83, 73, 54, 73, 107, 53, 49, 98, 71, 120, 87, 89, 87, 120, 49, 90, 84, 70, 107, 77, 87, 81, 49, 78, 122, 74, 107, 76, 87, 89, 52, 79, 84, 69, 116, 78, 68, 65, 121, 77, 105, 49, 104, 78, 122, 73, 48, 76, 84, 86, 108, 89, 109, 69, 50, 90, 84, 86, 107, 89, 84, 73, 120, 77, 67, 74, 57, 76, 72, 115, 105, 84, 109, 70, 116, 90, 83, 73, 54, 73, 107, 53, 104, 98, 87, 86, 109, 89, 122, 89, 49, 78, 71, 70, 107, 78, 67, 48, 50, 79, 68, 89, 122, 76, 84, 81, 50, 90, 84, 65, 116, 79, 84, 73, 51, 77, 121, 48, 122, 89, 122, 65, 119, 78, 109, 82, 104, 77, 106, 73, 121, 90, 68, 103, 105, 76, 67, 74, 87, 89, 87, 120, 49, 90, 83, 73, 54, 77, 84, 65, 120, 76, 67, 74, 79, 100, 87, 120, 115, 86, 109, 70, 115, 100, 87, 85, 105, 79, 105, 74, 79, 100, 87, 120, 115, 86, 109, 70, 115, 100, 87, 85, 52, 90, 87, 73, 52, 79, 84, 90, 108, 90, 67, 48, 53, 89, 50, 85, 119, 76, 84, 81, 120, 89, 109, 85, 116, 89, 106, 81, 49, 79, 83, 49, 108, 89, 87, 85, 122, 89, 106, 107, 48, 90, 109, 81, 53, 78, 68, 85, 105, 102, 86, 49, 57, 76, 72, 115, 105, 83, 87, 81, 105, 79, 106, 107, 120, 76, 67, 74, 79, 89, 87, 49, 108, 73, 106, 111, 105, 84, 109, 70, 116, 90, 87, 82, 105, 89, 106, 81, 49, 89, 122, 103, 50, 76, 84, 89, 48, 78, 71, 81, 116, 78, 68, 103, 53, 79, 67, 48, 53, 78, 50, 86, 106, 76, 84, 69, 50, 89, 50, 73, 122, 90, 106, 89, 49, 77, 50, 90, 109, 77, 105, 73, 115, 73, 107, 120, 108, 89, 88, 90, 108, 99, 121, 73, 54, 87, 51, 115, 105, 84, 109, 70, 116, 90, 83, 73, 54, 73, 107, 53, 104, 98, 87, 85, 48, 77, 106, 73, 52, 77, 84, 70, 107, 77, 121, 48, 119, 79, 87, 85, 52, 76, 84, 81, 120, 90, 87, 89, 116, 89, 87, 81, 121, 77, 121, 48, 121, 77, 122, 85, 119, 77, 106, 107, 50, 78, 68, 66, 107, 77, 122, 73, 105, 76, 67, 74, 87, 89, 87, 120, 49, 90, 83, 73, 54, 77, 84, 81, 48, 76, 67, 74, 79, 100, 87, 120, 115, 86, 109, 70, 115, 100, 87, 85, 105, 79, 105, 74, 79, 100, 87, 120, 115, 86, 109, 70, 115, 100, 87, 86, 109, 77, 122, 99, 48, 78, 106, 78, 106, 90, 67, 48, 119, 78, 50, 73, 51, 76, 84, 82, 105, 78, 109, 73, 116, 89, 109, 89, 52, 89, 105, 48, 52, 79, 71, 82, 108, 90, 68, 107, 122, 78, 106, 86, 105, 89, 106, 65, 105, 102, 83, 120, 55, 73, 107, 53, 104, 98, 87, 85, 105, 79, 105, 74, 79, 89, 87, 49, 108, 89, 84, 69, 121, 89, 50, 82, 105, 77, 84, 103, 116, 89, 109, 82, 108, 90, 67, 48, 48, 89, 106, 86, 109, 76, 87, 69, 50, 78, 106, 77, 116, 89, 84, 73, 119, 79, 68, 66, 107, 90, 84, 66, 107, 79, 87, 85, 52, 73, 105, 119, 105, 86, 109, 70, 115, 100, 87, 85, 105, 79, 106, 73, 48, 78, 105, 119, 105, 84, 110, 86, 115, 98, 70, 90, 104, 98, 72, 86, 108, 73, 106, 111, 105, 84, 110, 86, 115, 98, 70, 90, 104, 98, 72, 86, 108, 77, 87, 89, 51, 78, 122, 81, 120, 77, 122, 81, 116, 77, 68, 108, 108, 89, 105, 48, 48, 90, 87, 69, 52, 76, 87, 73, 50, 78, 71, 89, 116, 89, 122, 66, 108, 89, 122, 99, 48, 89, 106, 65, 50, 77, 84, 81, 51, 73, 110, 48, 115, 101, 121, 74, 79, 89, 87, 49, 108, 73, 106, 111, 105, 84, 109, 70, 116, 90, 87, 85, 48, 90, 84, 99, 122, 89, 122, 73, 52, 76, 87, 89, 119, 78, 84, 69, 116, 78, 71, 90, 108, 90, 83, 49, 104, 77, 87, 70, 104, 76, 84, 69, 48, 78, 109, 74, 105, 78, 84, 65, 53, 89, 87, 77, 52, 77, 83, 73, 115, 73, 108, 90, 104, 98, 72, 86, 108, 73, 106, 111, 120, 78, 106, 103, 115, 73, 107, 53, 49, 98, 71, 120, 87, 89, 87, 120, 49, 90, 83, 73, 54, 73, 107, 53, 49, 98, 71, 120, 87, 89, 87, 120, 49, 90, 84, 65, 48, 77, 50, 89, 51, 79, 71, 69, 122, 76, 87, 89, 53, 90, 68, 69, 116, 78, 68, 104, 106, 78, 83, 48, 52, 89, 122, 78, 104, 76, 84, 100, 104, 79, 84, 100, 106, 77, 106, 78, 107, 77, 122, 69, 119, 78, 83, 74, 57, 88, 88, 48, 115, 101, 121, 74, 74, 90, 67, 73, 54, 77, 106, 73, 122, 76, 67, 74, 79, 89, 87, 49, 108, 73, 106, 111, 105, 84, 109, 70, 116, 90, 87, 82, 108, 90, 84, 81, 119, 89, 122, 66, 105, 76, 87, 85, 49, 77, 68, 73, 116, 78, 68, 66, 106, 78, 105, 48, 52, 78, 106, 78, 104, 76, 84, 65, 51, 89, 87, 81, 48, 78, 71, 69, 51, 90, 87, 70, 104, 78, 83, 73, 115, 73, 107, 120, 108, 89, 88, 90, 108, 99, 121, 73, 54, 87, 51, 115, 105, 84, 109, 70, 116, 90, 83, 73, 54, 73, 107, 53, 104, 98, 87, 85, 52, 77, 68, 81, 49, 78, 68, 103, 52, 78, 105, 48, 51, 78, 68, 66, 107, 76, 84, 81, 121, 89, 109, 69, 116, 89, 106, 81, 121, 77, 67, 48, 49, 78, 106, 65, 119, 77, 84, 86, 104, 78, 68, 85, 48, 90, 84, 65, 105, 76, 67, 74, 87, 89, 87, 120, 49, 90, 83, 73, 54, 77, 84, 103, 50, 76, 67, 74, 79, 100, 87, 120, 115, 86, 109, 70, 115, 100, 87, 85, 105, 79, 105, 74, 79, 100, 87, 120, 115, 86, 109, 70, 115, 100, 87, 86, 106, 90, 109, 73, 50, 77, 122, 103, 51, 89, 105, 48, 49, 77, 106, 81, 120, 76, 84, 82, 105, 90, 109, 77, 116, 89, 106, 85, 122, 89, 121, 48, 51, 77, 106, 77, 52, 77, 84, 73, 48, 77, 68, 104, 107, 78, 84, 85, 105, 102, 83, 120, 55, 73, 107, 53, 104, 98, 87, 85, 105, 79, 105, 74, 79, 89, 87, 49, 108, 89, 106, 73, 49, 78, 84, 99, 53, 89, 50, 77, 116, 89, 50, 74, 107, 90, 83, 48, 48, 78, 68, 104, 105, 76, 84, 107, 119, 78, 122, 99, 116, 78, 50, 74, 106, 79, 87, 77, 51, 89, 87, 90, 105, 78, 68, 86, 107, 73, 105, 119, 105, 86, 109, 70, 115, 100, 87, 85, 105, 79, 106, 73, 48, 79, 67, 119, 105, 84, 110, 86, 115, 98, 70, 90, 104, 98, 72, 86, 108, 73, 106, 111, 105, 84, 110, 86, 115, 98, 70, 90, 104, 98, 72, 86, 108, 77, 84, 107, 53, 77, 106, 65, 122, 77, 50, 89, 116, 77, 71, 82, 107, 77, 121, 48, 48, 78, 87, 81, 121, 76, 84, 104, 104, 77, 109, 69, 116, 89, 84, 89, 119, 78, 122, 69, 50, 77, 122, 81, 119, 90, 84, 69, 50, 73, 110, 48, 115, 101, 121, 74, 79, 89, 87, 49, 108, 73, 106, 111, 105, 84, 109, 70, 116, 90, 87, 85, 52, 78, 71, 78, 106, 78, 84, 77, 50, 76, 87, 89, 119, 89, 50, 81, 116, 78, 68, 99, 121, 77, 83, 49, 104, 77, 109, 77, 120, 76, 84, 69, 48, 79, 84, 103, 122, 90, 68, 103, 48, 77, 71, 77, 53, 90, 105, 73, 115, 73, 108, 90, 104, 98, 72, 86, 108, 73, 106, 111, 121, 77, 84, 107, 115, 73, 107, 53, 49, 98, 71, 120, 87, 89, 87, 120, 49, 90, 83, 73, 54, 73, 107, 53, 49, 98, 71, 120, 87, 89, 87, 120, 49, 90, 84, 100, 108, 89, 122, 70, 104, 89, 84, 99, 119, 76, 84, 81, 119, 77, 84, 103, 116, 78, 68, 73, 122, 77, 105, 49, 104, 89, 50, 85, 120, 76, 84, 107, 50, 77, 68, 85, 50, 78, 71, 69, 121, 77, 122, 77, 53, 77, 105, 74, 57, 88, 88, 49, 100, 76, 67, 74, 85, 97, 87, 49, 108, 99, 51, 82, 104, 98, 88, 65, 105, 79, 105, 73, 121, 77, 68, 73, 49, 76, 84, 65, 50, 76, 84, 65, 52, 86, 68, 69, 51, 79, 106, 85, 51, 79, 106, 73, 119, 73, 105, 119, 105, 86, 109, 70, 115, 100, 87, 85, 105, 79, 106, 69, 121, 78, 51, 48, 61, 34, 125 };
    }
}
