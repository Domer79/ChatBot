const webpack = require('webpack');
console.log('Start Ignore plugin');
module.exports = {
    plugins: [
        new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
    ]
}
