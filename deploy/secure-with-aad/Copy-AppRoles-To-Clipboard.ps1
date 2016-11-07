# Prepare roles to change manifest with:
$guidForQuizMaker = [System.Guid]::NewGuid().ToString()
$guidForQuizTaker = [System.Guid]::NewGuid().ToString()

$roles = 
"{ `
    `"allowedMemberTypes`": [`"Application`"], `
    `"description`": `"Applications can administer quizzes`", `
    `"displayName`": `"QuizMaker`", `
    `"isEnabled`": true, `
    `"value`": `"quizmaker`", `
    `"id`": `"$guidForQuizMaker`" `
}, `
{ `
    `"allowedMemberTypes`": [`"Application`"], `
    `"description`": `"Applications can take attempts on quizzes`", `
    `"displayName`": `"QuizTaker`", `
    `"isEnabled`": true, `
    `"value`": `"quiztaker`", `
    `"id`": `"$guidForQuizTaker`" ` 
}"

$roles | clip

Write-Host "There is snippet copied on the clipboard for changing the roles in the AD application manifest for the API."