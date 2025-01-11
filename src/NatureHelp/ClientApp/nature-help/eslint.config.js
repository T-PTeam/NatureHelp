// https://eslint.org/docs/v8.x/

const config = {
  env: {
      browser: true,
      node: true,
      es2021: true,
  },
  extends: [
      'eslint:recommended',
      'plugin:react/recommended',
      'plugin:react-hooks/recommended',
      'plugin:import/errors',
      'plugin:import/warnings',
      'plugin:unicorn/recommended',
      'plugin:cypress/recommended',
  ],
  settings: {
      'import/resolver': {
          node: {
              extensions: ['.ts', '.jsx'],
          },
          webpack: {
              config: './config/webpack.dev.config.ts',
          },
      },
  },
  plugins: ['simple-import-sort', 'react'],
  ignorePatterns: ['node_modules'],
  rules: {
      'unicorn/filename-case': [
          'error',
          {
              cases: {
                  camelCase: true,
                  pascalCase: true,
              },
          },
      ],
      'unicorn/no-empty-file': 'off',
      'simple-import-sort/exports': 'error',
      'simple-import-sort/imports': 'error',
      'import/namespace': [2, { allowComputed: true }],
      'import/first': 'error',
      'import/newline-after-import': 'error',
  },
  overrides: [
      {
          files: ['*rc.ts', '*.config.ts'],
          rules: {
              'unicorn/prefer-module': 'off',
              'unicorn/filename-case': 'off',
          },
      },
  ],
  globals: {
      Cypress: true,
  },
};

module.exports = config;