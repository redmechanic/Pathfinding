﻿Public Class AStarSearch

    Public moveCost As Integer = 10

    Public Sub calculateHCost(ByRef cell As Cell, finish As Point)
        Dim start As Point = cell.getIndexXY()
        cell.hCost = (Math.Abs(start.X - finish.X) + Math.Abs(start.Y - finish.Y)) * moveCost
    End Sub

    Public Sub calculateGCost(ByRef cell As Cell)
        Try
            cell.gCost = cell.parent.gCost + moveCost
        Catch
            cell.gCost = moveCost
        End Try
    End Sub

    Public Sub calculateFCost(ByRef cell As Cell)
        cell.fCost = cell.gCost + cell.hCost
    End Sub

    Public Sub drawCells(cells As List(Of Cell), color As System.Drawing.Color, display As VBGame)
        For Each Cell As Cell In cells
            display.drawRect(Cell.getRect(), color)
            display.drawText(Cell.getRect(), Cell.fCost, VBGame.black, New Font("Ariel", 8))
        Next
    End Sub

    Public Function getNeighbors(grid As Grid, current As Cell) As List(Of Cell)
        Dim neighbors As New List(Of Cell)
        Dim currentPoint = current.getIndexXY()

        For x As Integer = -1 To 1
            For y As Integer = -1 To 1
                If (x = 0 OrElse y = 0) AndAlso (Not (x = 0 AndAlso y = 0)) Then
                    Try
                        neighbors.Add(grid.cells(currentPoint.X + x, currentPoint.Y + y))
                    Catch
                    End Try
                End If
            Next
        Next
        Return neighbors
    End Function

    Public Sub drawLine(current As Cell, display As VBGame)
        Try
            display.drawLine(New Point(current.x + (current.side / 2), current.y + (current.side / 2)), New Point(current.parent.x + (current.parent.side / 2), current.parent.y + (current.parent.side / 2)), VBGame.blue, current.side / 5)
            display.update()
            display.clockTick(15)
            drawLine(current.parent, display)
        Catch
        End Try
    End Sub

    Public Function searchLoop(grid As Grid, display As VBGame) As Boolean

        Dim open As New List(Of Cell)
        Dim closed As New List(Of Cell)

        Dim current As Cell

        Dim lowest As Integer

        open.Add(grid.cells(grid.startpoint.X, grid.startpoint.Y))
        open(0).fCost = 9999

        current = open(0)

        grid.drawAllCells(display)

        While True

            lowest = 9999
            For Each Cell As Cell In open

                If Cell.fCost < lowest Then
                    lowest = Cell.fCost
                    current = Cell
                End If

            Next

            closed.Add(current)
            display.drawRect(current.getRect(), VBGame.red)

            open.Remove(current)

            If current.getIndexXY() = grid.finishpoint Then
                drawLine(current, display)
                Return True
            End If

            For Each Cell As Cell In getNeighbors(grid, current)
                If Not IsNothing(Cell) AndAlso Not closed.Contains(Cell) AndAlso Cell.state <> Cell.States.Wall Then

                    If Not open.Contains(Cell) Or current.gCost + moveCost < Cell.gCost Then

                        Cell.gCost = current.gCost + moveCost

                        calculateHCost(Cell, grid.finishpoint)
                        calculateFCost(Cell)

                        Cell.parent = current

                        If Not open.Contains(Cell) Then
                            open.Add(Cell)
                            display.drawRect(Cell.getRect(), VBGame.green)
                        End If

                    End If

                End If
            Next

            'drawCells(open, VBGame.green, display)
            'drawCells(closed, VBGame.red, display)

            'display.drawRect(current.getRect(), Color.Purple)

            'drawLine(current, display)

            display.update()
            display.clockTick(60)

        End While

        Return True

    End Function

End Class