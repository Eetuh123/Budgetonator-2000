## Workflow

1. **Create/Choose Linear Issue:** Start by creating a new issue in Linear or selecting an existing one that your work will address.

2. **Copy Branch Name:** Copy the suggested Git branch name from the Linear issue.

3. **Create Branch:** Create a new local branch using the copied name: `git checkout -b copied_branch_name`.

4. **Work:** Complete the necessary work.

5. **Commit Changes:** Stage and commit your changes with a concise, descriptive message: `git add .` then `git commit -m "Short description of your changes"`.

6. **Push Branch:** Push your branch to the remote repository: `git push --set-upstream origin copied_branch_name` (first time) and then subsequent `git push`.

7. **Review:** Do not merge your branch until we have reviewed the changes together on Wednesday.
