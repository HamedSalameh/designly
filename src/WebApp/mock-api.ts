const jsonServer = require('json-server')
const server = jsonServer.create()
const router = jsonServer.router('db.json')
const middlewares = jsonServer.defaults()

// Set default middlewares (logger, static, cors and no-cache)
server.use(middlewares);

// Parse JSON request bodies
server.use(jsonServer.bodyParser);

// Custom route handlers

// Mock login endpoint
server.post('/api/signin', (req, res) => {
  const response = {
    idToken: 'eyJraWQiOiJGc01jSTRYeUJVSHd1bGtSTVg5QVYySzg0NDV2ek9RTUFCeHI5emxaYk5BPSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiJjMDdkOWFlZC1jNWJjLTRjM2QtODg2MS02ZWZkYmEyNDlmOGYiLCJpc3MiOiJodHRwczpcL1wvY29nbml0by1pZHAudXMtZWFzdC0xLmFtYXpvbmF3cy5jb21cL3VzLWVhc3QtMV9NUWVSaG9lZjIiLCJjb2duaXRvOnVzZXJuYW1lIjoiYzA3ZDlhZWQtYzViYy00YzNkLTg4NjEtNmVmZGJhMjQ5ZjhmIiwiZ2l2ZW5fbmFtZSI6IkhhbWVkIiwib3JpZ2luX2p0aSI6IjU4NTE3ZDZmLTBkZTEtNDI2Ny05Nzg2LWFmNTc3NTkzNzQ0NCIsImF1ZCI6IjcwOXMzdXBnbjFicmFqZWE5ajNncGxoM2dtIiwiZXZlbnRfaWQiOiI1ODIwMjI2Ni1mNTEwLTRjN2ItYjVlNS1hYzQyMzAwODkyNDYiLCJ0b2tlbl91c2UiOiJpZCIsImF1dGhfdGltZSI6MTY4NDQ3MTEwMCwiZXhwIjoxNjg0NDcxNDAwLCJpYXQiOjE2ODQ0NzExMDAsImZhbWlseV9uYW1lIjoiU2FsYW1laCIsImp0aSI6IjM2NjI0MTgzLTU3ZmQtNDFlNS1hYmVlLTYwNWFhMjA3Y2VkOCIsImVtYWlsIjoiaGFtZWRzYWxhbWlAZ21haWwuY29tIn0.evUwyGxKzH5bUKTtBI4VMtjz7oZmML9eD_xIofcLvKsKYL-ZAedlRC83Ocu1zXzNYwGcj25pPkWvN7coNH41yC8aSb6lPr0L-LxczGf_AM1rV3cFO_FZoCi7uqcABy2TJzqQxGr-gxh7cSPFOM3oIAHGikTjBk-Hh-0qMW05_0ZSVpfXiJ7MMVaDipvSU4gJ3Zt4rSrOcfj7ho92OoxtmkoaomYh2otPhjtqzFVeNsZTI_cffNc_Ncd6YeQxrA7zhAh4bpCwmayiIi6mXl8DqPS8EalKUlgjPvy_WbT4G3XqbUSYwAZFidVoacXEZMNSveN1rmLFmRxGnodZ-Hv4dQ',
    refreshToken: 'Not Set',
    accessToken: 'eyJraWQiOiJIaHRZQ0UwRVd1eW04RHNBZ09LXC9qZm5ka0NDTURZN0pwaktieWZzVWtvcz0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJjMDdkOWFlZC1jNWJjLTRjM2QtODg2MS02ZWZkYmEyNDlmOGYiLCJpc3MiOiJodHRwczpcL1wvY29nbml0by1pZHAudXMtZWFzdC0xLmFtYXpvbmF3cy5jb21cL3VzLWVhc3QtMV9NUWVSaG9lZjIiLCJjbGllbnRfaWQiOiI3MDlzM3VwZ24xYnJhamVhOWozZ3BsaDNnbSIsIm9yaWdpbl9qdGkiOiI1ODUxN2Q2Zi0wZGUxLTQyNjctOTc4Ni1hZjU3NzU5Mzc0NDQiLCJldmVudF9pZCI6IjU4MjAyMjY2LWY1MTAtNGM3Yi1iNWU1LWFjNDIzMDA4OTI0NiIsInRva2VuX3VzZSI6ImFjY2VzcyIsInNjb3BlIjoiYXdzLmNvZ25pdG8uc2lnbmluLnVzZXIuYWRtaW4iLCJhdXRoX3RpbWUiOjE2ODQ0NzExMDAsImV4cCI6MTY4NDQ3MTQwMCwiaWF0IjoxNjg0NDcxMTAwLCJqdGkiOiJhNmI5N2U4Zi0yZDU0LTQxMmUtOTJhYi04NjY3ZDVmMzlhMGQiLCJ1c2VybmFtZSI6ImMwN2Q5YWVkLWM1YmMtNGMzZC04ODYxLTZlZmRiYTI0OWY4ZiJ9.vQmAgnqcRs2fDkv58vcvDgaE0HOnnry-6SGKn2REoC17jpRC8SOnF3DZQbOHDwm8Z-3ElqTfaPYCEa50DO0BY1zrmm6DH-P5nc-dAiKylVI6RPZARIihb46ZygSTw7Q8G6AKqaZmUdrB2KihjtJDB0oppxp3mOn3pXZG4AG3pNh7L90LPaM6y3Xnz9ZuW_1R3tlUcsegpPKzvRiFYbbi14KKbe9rcnowG_pyb5QkYH1WLKzMJgUELbH7XdylrUxtB7hgYIKNXMV6le1pvCs2XnraWyfQ10hlhEoDUzgBbNVY1QJ2maA_Xw3bEZEjOCoOfKIWQs5MjjCaIvkw2Fdq3Q',
    expiresIn: 300,
    tokenType: 'Bearer',
  };

  res.json(response);
});


// Custom route handler for getting a client by ID
server.get('/clients/:id', (req, res) => {
  const clientId = req.params.id;
  const client = router.db
    .get('clients')
    .find({ Id: clientId })
    .value();

  if (client) {
    res.json(client);
  } else {
    res.status(404).json({ error: 'Client not found' });
  }
});

// Use the custom routes
server.use(router);

// Start the server
const port = 3000;
server.listen(port, () => {
  console.log(`Mock API Server is running on port ${port}`);
});
