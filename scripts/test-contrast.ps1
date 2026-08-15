# ============================================================
# SCRIPT PARA PRUEBAS DE CONTRASTE (VB6 vs C#)
# ============================================================
# Compara resultados entre el WS original y el nuevo API REST

param(
    [string]$WsUrl = "http://localhost/BioServerWS/BioServerWS.asmx",
    [string]$ApiUrl = "http://localhost:5000/api/bioserver",
    [string]$TestDataPath = "..\tests\contrast\test-data\input"
)

Write-Host "🧪 Iniciando pruebas de contraste..." -ForegroundColor Cyan

# 1. Verificar que los endpoints estén disponibles
Write-Host "🔍 Verificando disponibilidad de servicios..." -ForegroundColor Yellow
try {
    $wsCheck = Invoke-WebRequest -Uri "$WsUrl" -Method Head -TimeoutSec 5 -ErrorAction SilentlyContinue
    Write-Host "✅ WS original disponible: $WsUrl" -ForegroundColor Green
} catch {
    Write-Host "❌ Error: WS original no disponible en $WsUrl" -ForegroundColor Red
    exit 1
}

try {
    $apiCheck = Invoke-WebRequest -Uri "$ApiUrl/send-to-server" -Method Options -TimeoutSec 5 -ErrorAction SilentlyContinue
    Write-Host "✅ API REST disponible: $ApiUrl" -ForegroundColor Green
} catch {
    Write-Host "❌ Error: API REST no disponible en $ApiUrl" -ForegroundColor Red
    exit 1
}

# 2. Cargar los casos de prueba
Write-Host "📂 Cargando casos de prueba..." -ForegroundColor Yellow
$testCases = Get-ChildItem -Path $TestDataPath -Filter "*.json" -ErrorAction SilentlyContinue

if ($testCases.Count -eq 0) {
    Write-Host "⚠️ No se encontraron casos de prueba en $TestDataPath" -ForegroundColor Yellow
    Write-Host "💡 Cree archivos .json con los payloads de prueba" -ForegroundColor Gray
    exit 1
}

Write-Host "✅ Encontrados $($testCases.Count) casos de prueba" -ForegroundColor Green

# 3. Ejecutar las pruebas
$results = @()
$passed = 0
$failed = 0

foreach ($testCase in $testCases) {
    Write-Host ""
    Write-Host "📝 Caso de prueba: $($testCase.Name)" -ForegroundColor White
    
    # Leer el payload
    $payload = Get-Content -Path $testCase.FullName -Raw | ConvertFrom-Json
    
    # Llamar al WS original
    $wsPayload = @{
        id = $payload.id
        secret = $payload.secret
        payload = $payload.payload
    }
    $wsBody = $wsPayload | ConvertTo-Json
    
    try {
        $wsResult = Invoke-WebRequest -Uri "$WsUrl/SendToServer" -Method Post `
            -Body $wsBody -ContentType "application/json" -TimeoutSec 30
        $wsResponse = $wsResult.Content | ConvertFrom-Json
        Write-Host "✅ WS original: OK" -ForegroundColor Green
    } catch {
        Write-Host "❌ WS original: ERROR - $($_.Exception.Message)" -ForegroundColor Red
        $wsResponse = $null
    }
    
    # Llamar al API REST
    $apiBody = @{
        id = $payload.id
        secret = $payload.secret
        payload = $payload.payload
    } | ConvertTo-Json
    
    try {
        $apiResult = Invoke-WebRequest -Uri "$ApiUrl/send-to-server" -Method Post `
            -Body $apiBody -ContentType "application/json" -TimeoutSec 30
        $apiResponse = $apiResult.Content | ConvertFrom-Json
        Write-Host "✅ API REST: OK" -ForegroundColor Green
    } catch {
        Write-Host "❌ API REST: ERROR - $($_.Exception.Message)" -ForegroundColor Red
        $apiResponse = $null
    }
    
    # Comparar resultados
    $testPassed = $true
    if ($wsResponse -and $apiResponse) {
        $wsData = $wsResponse.Data | ConvertFrom-Json
        $apiData = $apiResponse.Data | ConvertFrom-Json
        
        # Comparar properties principales
        if ($wsData.result -ne $apiData.result) {
            Write-Host "❌ Diferencia: result = $($wsData.result) vs $($apiData.result)" -ForegroundColor Red
            $testPassed = $false
        }
        
        if ($wsData.err_num -ne $apiData.err_num) {
            Write-Host "❌ Diferencia: err_num = $($wsData.err_num) vs $($apiData.err_num)" -ForegroundColor Red
            $testPassed = $false
        }
        
        if ($wsData.err_msg -ne $apiData.err_msg) {
            Write-Host "❌ Diferencia: err_msg = $($wsData.err_msg) vs $($apiData.err_msg)" -ForegroundColor Red
            $testPassed = $false
        }
    } else {
        $testPassed = $false
        Write-Host "❌ No se pudo comparar (uno de los servicios falló)" -ForegroundColor Red
    }
    
    if ($testPassed) {
        $passed++
        Write-Host "✅ Prueba PASADA" -ForegroundColor Green
    } else {
        $failed++
        Write-Host "❌ Prueba FALLIDA" -ForegroundColor Red
    }
    
    $results += [PSCustomObject]@{
        Test = $testCase.Name
        Passed = $testPassed
        WsResponse = $wsResponse
        ApiResponse = $apiResponse
    }
}

# 4. Generar resumen
Write-Host ""
Write-Host "📊 RESUMEN DE PRUEBAS" -ForegroundColor Cyan
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Gray
Write-Host "✅ Pruebas exitosas: $passed" -ForegroundColor Green
Write-Host "❌ Pruebas fallidas: $failed" -ForegroundColor Red
Write-Host "📊 Total: $($testCases.Count)" -ForegroundColor White
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Gray

if ($failed -gt 0) {
    Write-Host "⚠️ Hay pruebas fallidas. Revise los resultados detallados." -ForegroundColor Yellow
    exit 1
} else {
    Write-Host "✅ Todas las pruebas pasaron exitosamente." -ForegroundColor Green
    exit 0
}