#!/usr/bin/env ruby
# Encoding: utf-8
#
# Copyright:: Copyright 2016, Google Inc. All Rights Reserved.
#
# License:: Licensed under the Apache License, Version 2.0 (the "License");
#           you may not use this file except in compliance with the License.
#           You may obtain a copy of the License at
#
#           http://www.apache.org/licenses/LICENSE-2.0
#
#           Unless required by applicable law or agreed to in writing, software
#           distributed under the License is distributed on an "AS IS" BASIS,
#           WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
#           implied.
#           See the License for the specific language governing permissions and
#           limitations under the License.
#
# This example creates a campaign in a given advertiser.
#
# To create an advertiser, run create_advertiser.rb.

require_relative '../dfareporting_utils'
require 'securerandom'

def create_campaign(profile_id, advertiser_id)
  # Authenticate and initialize API service.
  service = DfareportingUtils.get_service()

  # Locate an advertiser landing page to use as a default.
  default_landing_page = get_advertiser_landing_page(service, profile_id,
      advertiser_id)

  # Create a new campaign resource to insert.
  campaign = DfareportingUtils::API_NAMESPACE::Campaign.new({
    :advertiser_id => advertiser_id,
    :archived => false,
    :default_landing_page_id => default_landing_page.id,
    :name => 'Example Campaign #%s' % SecureRandom.hex(3),
    :start_date => '2014-01-01',
    :end_date => '2020-01-01'
  })

  # Insert the campaign.
  result = service.insert_campaign(profile_id, campaign)

  # Display results.
  puts 'Created campaign with ID %d and name "%s".' %
      [result.id, result.name]
end

def get_advertiser_landing_page(service, profile_id, advertiser_id)
  # Retrieve a sigle landing page from the specified advertiser.
  result = service.list_advertiser_landing_pages(profile_id, {
    :advertiser_ids => [advertiser_id],
    :max_results => 1
  })

  if !result.landing_pages.any?
    abort 'No landing pages for for advertiser with ID %d' % advertiser_id
  end

  return result.landing_pages[0]
end

if __FILE__ == $0
  # Retrieve command line arguments.
  args = DfareportingUtils.get_arguments(ARGV, :profile_id, :advertiser_id)

  create_campaign(args[:profile_id], args[:advertiser_id])
end
