#!/usr/bin/env bash

source="${BASH_SOURCE[0]}"

# resolve $SOURCE until the file is no longer a symlink
while [[ -h $source ]]; do
  scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"
  source="$(readlink "$source")"

  # if $source was a relative symlink, we need to resolve it relative to the path where 
  # the symlink file was located
  [[ $source != /* ]] && source="$scriptroot/$source"
done
scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"
"$scriptroot/eng/common/build.sh" --clean
# calculate OfficialBuildId in (yyyyMMDD.1) format
OfficialBuildId=$(date -u +%Y%m%d).1
"$scriptroot/eng/common/build.sh" --restore --build --test --pack --ci /p:OfficialBuildId="$OfficialBuildId" "$@"

# find the nupkg file we just built in the artifacts folder
# file=$(find "$scriptroot/artifacts" -name '*.nupkg' | head -n 1)
# open "$file"
dotnet nuget push -s "$NUGET_FEED" -k "$NUGET_API_KEY" "$scriptroot/artifacts/**/*.nupkg"