module.exports = {
    "*.ts": ["npx prettier --write", "npx eslint --fix"],
    "*.js": ["npx prettier --write", "npx eslint --fix"],
};
