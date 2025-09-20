#!/bin/sh

#### Analyze code

SHIFTLEFT_SBOM_GENERATOR=2 sl analyze \
  --app "$CI_PROJECT_NAME" \
  --tag branch="$CI_COMMIT_REF_NAME" \
  --wait \
  DataMocker.sln

#### Run build rules

# Check if this is running in a merge request
if [ -n "$CI_MERGE_REQUEST_IID" ]; then
  echo "Got merge request $CI_MERGE_REQUEST_IID for branch $CI_COMMIT_REF_NAME"

  # Run check-analysis and save report to /tmp/check-analysis.md
  sl check-analysis \
    --app "$CI_PROJECT_NAME" \
    --report \
    --report-file /tmp/check-analysis.md \
    --source "tag.branch=master" \
    --target "tag.branch=$CI_COMMIT_REF_NAME"

  CHECK_ANALYSIS_OUTPUT=$(cat /tmp/check-analysis.md)
  COMMENT_BODY=$(jq -n --arg body "$CHECK_ANALYSIS_OUTPUT" '{body: $body}')

  # Post report as merge request comment
  curl -i -XPOST "https://gitlab.com/api/v4/projects/$CI_PROJECT_ID/merge_requests/$CI_MERGE_REQUEST_IID/notes" \
    -H "PRIVATE-TOKEN: $MR_TOKEN" \
    -H "Content-Type: application/json" \
    -d "$COMMENT_BODY"
fi