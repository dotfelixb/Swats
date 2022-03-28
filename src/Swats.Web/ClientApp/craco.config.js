const CracoLessPlugin = require("craco-less");

module.exports = {
  plugins: [
    {
      plugin: CracoLessPlugin,
      options: {
        lessLoaderOptions: {
          lessOptions: {
            modifyVars: {
              "@primary-color": "#3b2fc9",
              "@font-family": "Fredoka",
            },
            javascriptEnabled: true,
          },
        },
      },
    },
  ],
};
