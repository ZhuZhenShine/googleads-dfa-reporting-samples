﻿/*
 * Copyright 2015 Google Inc
 *
 * Licensed under the Apache License, Version 2.0(the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Linq;
using Google.Apis.Dfareporting.v2_2;
using Google.Apis.Dfareporting.v2_2.Data;

namespace DfaReporting.Samples {
  /// <summary>
  /// This example displays all available content categories.
  /// </summary>
  class GetContentCategories : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example displays all available content categories.\n";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main(string[] args) {
      SampleBase codeExample = new GetContentCategories();
      Console.WriteLine(codeExample.Description);
      codeExample.Run(DfaReportingFactory.getInstance());
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    /// <param name="service">An initialized Dfa Reporting service object
    /// </param>
    public override void Run(DfareportingService service) {
      long profileId = long.Parse(_T("INSERT_USER_PROFILE_ID_HERE"));

      // Limit the fields returned.
      String fields = "nextPageToken,contentCategories(id,name)";

      ContentCategoriesListResponse categories;
      String nextPageToken = null;

      do {
        // Create and execute the content categories list request
        ContentCategoriesResource.ListRequest request =
            service.ContentCategories.List(profileId);
        request.Fields = fields;
        request.PageToken = nextPageToken;
        categories = request.Execute();

        foreach (ContentCategory category in categories.ContentCategories) {
          Console.WriteLine("Found content category with ID {0} and name \"{1}\".",
              category.Id, category.Name);
        }

        // Update the next page token
        nextPageToken = categories.NextPageToken;
      } while (categories.ContentCategories.Any() && !String.IsNullOrEmpty(nextPageToken));
    }
  }
}
