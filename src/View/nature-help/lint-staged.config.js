module.exports = {
  '*.{ts}': ['eslint --ignore-path .gitignore --fix --ext .ts --max-warnings 0'],
  '*.{ts,css,html}': ['prettier --write'],
}