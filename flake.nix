{
  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/master";
    flake-utils.url = "github:numtide/flake-utils";
    flake-compat = {
      url = "github:edolstra/flake-compat";
      flake = false;
    };
  };

  outputs = { self, nixpkgs, flake-utils, ... }:
    flake-utils.lib.eachDefaultSystem (system:
      let
        pkgs = import nixpkgs {
          inherit system;
        };
      in
      with pkgs;
      {
        devShells.default = mkShell rec {
          dotnet-pkg = (with dotnetCorePackages; combinePackages [
            sdk_7_0
          ]);

          packages = 
            let
              dotnet-tools = (callPackage ./dotnet-tool.nix {});
            in [
              dotnet-pkg
              (dotnet-tools.combineTools dotnet-pkg (with dotnet-tools.tools; [
                fsautocomplete
                dotnet-repl
              ]))
            ];

          shellHook = ''
            DOTNET_ROOT="${dotnet-pkg}";
            PATH="~/.dotnet/tools:$PATH";
          '';
        };
      }
    );
}
